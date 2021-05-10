﻿using System.Linq;
using EVIL.Grammar.AST;
using EVIL.Grammar.AST.Nodes;
using EVIL.Lexical;

namespace EVIL.Grammar.Parsing
{
    public partial class Parser
    {
        private static readonly TokenType[] _logicalExpressionOperators = new[]
        {
            TokenType.And,
            TokenType.Or
        };

        private AstNode LogicalExpression()
        {
            var node = Comparison();
            var token = Scanner.State.CurrentToken;

            while (_logicalExpressionOperators.Contains(token.Type))
            {
                token = Scanner.State.CurrentToken;

                if (token.Type == TokenType.Or)
                {
                    var line = Match(TokenType.Or);
                    node = new BinaryOperationNode(node, Comparison(), BinaryOperationType.Or) { Line = line };
                }
                else if (token.Type == TokenType.And)
                {
                    var line = Match(TokenType.And);
                    node = new BinaryOperationNode(node, Comparison(), BinaryOperationType.And) { Line = line };
                }
            }
            
            return node;
        }
    }
}