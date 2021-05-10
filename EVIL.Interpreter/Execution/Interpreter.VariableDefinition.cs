﻿using EVIL.Grammar.AST.Nodes;
using EVIL.Interpreter.Abstraction;

namespace EVIL.Interpreter.Execution
{
    public partial class Interpreter
    {
        public override DynValue Visit(VarNode varNode)
        {
            var identifier = varNode.Identifier;

            if (Environment.LocalScope.HasMember(identifier))
            {
                throw new RuntimeException(
                    $"Variable '{identifier}' was already defined in the current scope.", 
                    Environment,
                    varNode.Line
                );
            }

            var dynValue = Environment.LocalScope.Set(identifier, DynValue.Zero);
            dynValue.CopyFrom(Visit(varNode.Right));

            return dynValue;
        }
    }
}