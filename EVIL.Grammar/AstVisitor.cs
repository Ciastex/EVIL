﻿using System;
using System.Collections.Generic;
using EVIL.Grammar.AST;
using EVIL.Grammar.AST.Nodes;

namespace EVIL.Grammar
{
    public abstract class AstVisitor
    {
        private Dictionary<Type, NodeHandler> _handlers;

        protected delegate void NodeHandler(AstNode node);

        protected AstVisitor()
        {
            _handlers = new()
            {
                {typeof(ProgramNode), (n) => Visit(n as ProgramNode)},
                {typeof(BlockStatementNode), (n) => Visit(n as BlockStatementNode)},
                {typeof(ConditionalExpressionNode), (n) => Visit(n as ConditionalExpressionNode)},
                {typeof(DecimalNode), (n) => Visit(n as DecimalNode)},
                {typeof(IntegerNode), (n) => Visit(n as IntegerNode)},
                {typeof(StringNode), (n) => Visit(n as StringNode)},
                {typeof(AssignmentNode), (n) => Visit(n as AssignmentNode)},
                {typeof(BinaryOperationNode), (n) => Visit(n as BinaryOperationNode)},
                {typeof(UnaryOperationNode), (n) => Visit(n as UnaryOperationNode)},
                {typeof(VariableNode), (n) => Visit(n as VariableNode)},
                {typeof(VariableDefinitionNode), (n) => Visit(n as VariableDefinitionNode)},
                {typeof(FunctionDefinitionNode), (n) => Visit(n as FunctionDefinitionNode)},
                {typeof(FunctionCallNode), (n) => Visit(n as FunctionCallNode)},
                {typeof(ConditionNode), (n) => Visit(n as ConditionNode)},
                {typeof(ExitNode), (n) => Visit(n as ExitNode)},
                {typeof(ForLoopNode), (n) => Visit(n as ForLoopNode)},
                {typeof(DoWhileLoopNode), (n) => Visit(n as DoWhileLoopNode)},
                {typeof(WhileLoopNode), (n) => Visit(n as WhileLoopNode)},
                {typeof(ReturnNode), (n) => Visit(n as ReturnNode)},
                {typeof(BreakNode), (n) => Visit(n as BreakNode)},
                {typeof(SkipNode), (n) => Visit(n as SkipNode)},
                {typeof(TableNode), (n) => Visit(n as TableNode)},
                {typeof(IndexingNode), (n) => Visit(n as IndexingNode)},
                {typeof(IncrementationNode), (n) => Visit(n as IncrementationNode)},
                {typeof(DecrementationNode), (n) => Visit(n as DecrementationNode)},
                {typeof(UndefNode), (n) => Visit(n as UndefNode)},
                {typeof(EachLoopNode), (n) => Visit(n as EachLoopNode)},
            };
        }

        public void Visit(AstNode node)
        {
            var type = node.GetType();

            if (!_handlers.ContainsKey(type))
                throw new Exception("Forgot to add a node type, idiot.");

            _handlers[type](node);
        }

        public abstract void Visit(ProgramNode programNode);
        public abstract void Visit(BlockStatementNode blockStatementNode);
        public abstract void Visit(ConditionalExpressionNode conditionalExpressionNode);
        public abstract void Visit(DecimalNode decimalNode);
        public abstract void Visit(IntegerNode integerNode);
        public abstract void Visit(StringNode stringNode);
        public abstract void Visit(AssignmentNode assignmentNode);
        public abstract void Visit(BinaryOperationNode binaryOperationNode);
        public abstract void Visit(UnaryOperationNode unaryOperationNode);
        public abstract void Visit(VariableNode variableNode);
        public abstract void Visit(VariableDefinitionNode variableDefinitionNode);
        public abstract void Visit(FunctionDefinitionNode scriptFunctionDefinitionNode);
        public abstract void Visit(FunctionCallNode functionCallNode);
        public abstract void Visit(ConditionNode conditionNode);
        public abstract void Visit(ExitNode exitNode);
        public abstract void Visit(ForLoopNode forLoopNode);
        public abstract void Visit(DoWhileLoopNode doWhileLoopNode);
        public abstract void Visit(WhileLoopNode whileLoopNode);
        public abstract void Visit(ReturnNode returnNode);
        public abstract void Visit(BreakNode breakNode);
        public abstract void Visit(SkipNode nextNode);
        public abstract void Visit(TableNode tableNode);
        public abstract void Visit(IndexingNode indexingNode);
        public abstract void Visit(IncrementationNode incrementationNode);
        public abstract void Visit(DecrementationNode decrementationNode);
        public abstract void Visit(UndefNode undefNode);
        public abstract void Visit(EachLoopNode eachLoopNode);
    }
}