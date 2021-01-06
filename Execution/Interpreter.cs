﻿using System.Collections.Generic;
using System.Threading.Tasks;
using EVIL.Abstraction;
using EVIL.AST.Base;
using EVIL.AST.Nodes;
using EVIL.Diagnostics;
using EVIL.Parsing;

namespace EVIL.Execution
{
    public partial class Interpreter : AstVisitor
    {
        public delegate DynValue ClrFunction(Interpreter interpreter, ClrFunctionArguments args);

        public bool BreakExecution { get; set; }

        public int CallStackLimit { get; set; } = 256;

        public Stack<CallStackItem> CallStack { get; }
        public Stack<LoopStackItem> LoopStack { get; }

        public Environment Environment { get; set; }

        public IMemory Memory { get; set; }
        public Parser Parser { get; }

        public Interpreter()
        {
            CallStack = new Stack<CallStackItem>();
            LoopStack = new Stack<LoopStackItem>();

            Environment = new Environment(this);
            Parser = new Parser();
        }

        public Interpreter(Environment env)
        {
            Environment = env;
        }

        public DynValue Execute(string sourceCode)
        {
            Parser.LoadSource(sourceCode);
            var node = Parser.Parse();

            try
            {
                return Visit(node);
            }
            catch (ExitStatementException)
            {
                CallStack.Clear();
                LoopStack.Clear();

                return DynValue.Zero;
            }
        }

        public async Task<DynValue> ExecuteAsync(string sourceCode)
        {
            Parser.LoadSource(sourceCode);
            var node = Parser.Parse();

            try
            {
                return await Task.Run(() => Visit(node));
            }
            catch (ExitStatementException)
            {
                CallStack.Clear();
                LoopStack.Clear();

                return DynValue.Zero;
            }
        }

        public async Task<DynValue> ExecuteAsync(string sourceCode, string entryPoint, string[] args)
        {
            Parser.LoadSource(sourceCode);
            var node = Parser.Parse();

            try
            {
                await Task.Run(() => Visit(node));
                var entryNode = node.FindChildFunctionDefinition(entryPoint);

                if (entryNode == null)
                {
                    throw new RuntimeException($"Entry point '{entryPoint}' missing.", null);
                }

                var csi = new CallStackItem(entryNode.Name);
                if (entryNode.ParameterNames.Count >= 1)
                {
                    var tbl = new Table();
                    for (var i = 0; i < args.Length; i++)
                    {
                        tbl[i] = new DynValue(args[i]);
                    }

                    csi.ParameterScope.Add(entryNode.ParameterNames[0], new DynValue(tbl));
                }
                CallStack.Push(csi);
                var result = ExecuteStatementList(entryNode.StatementList);

                if (CallStack.Count > 0)
                {
                    CallStack.Pop();
                }

                return result;
            }
            catch (ExitStatementException)
            {
                CallStack.Clear();
                LoopStack.Clear();

                return DynValue.Zero;
            }
        }

        public override DynValue Visit(RootNode rootNode)
        {
            ExecuteStatementList(rootNode.Children);
            return DynValue.Zero;
        }

        public List<CallStackItem> StackTrace()
        {
            return new(CallStack);
        }

        private DynValue ExecuteStatementList(List<AstNode> statements)
        {
            var retVal = DynValue.Zero;

            if (BreakExecution)
            {
                BreakExecution = false;
                throw new ProgramTerminationException("Execution stopped by user.");
            }

            foreach (var statement in statements)
            {
                if (BreakExecution)
                {
                    BreakExecution = false;
                    throw new ProgramTerminationException("Execution stopped by user.");
                }

                if (LoopStack.Count > 0)
                {
                    var loopStackTop = LoopStack.Peek();

                    if (loopStackTop.BreakLoop || loopStackTop.SkipThisIteration)
                        break;
                }

                if (CallStack.Count > 0)
                {
                    var callStackTop = CallStack.Peek();

                    if (callStackTop.ReturnNow)
                    {
                        retVal = callStackTop.ReturnValue;
                        break;
                    }
                }

                retVal = Visit(statement);
            }

            return retVal;
        }
    }
}