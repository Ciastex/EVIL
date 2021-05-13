﻿using System;
using EVIL.Grammar.AST;
using EVIL.Grammar.AST.Nodes;
using EVIL.Lexical;

namespace EVIL.Grammar.Parsing
{
    public partial class Parser
    {
        public AstNode PrimaryExpression()
        {
            var token = Scanner.State.CurrentToken;

            if (token.Type == TokenType.Decimal)
            {
                var line = Match(TokenType.Decimal);
                return new DecimalNode((decimal)token.Value) {Line = line};
            }
            else if (token.Type == TokenType.HexInteger)
            {
                var line = Match(TokenType.HexInteger);
                return new IntegerNode((int)token.Value) {Line = line};
            }
            else if (token.Type == TokenType.Integer)
            {
                var line = Match(TokenType.Integer);
                return new IntegerNode((int)token.Value) {Line = line};
            }
            else if (token.Type == TokenType.True)
            {
                var line = Match(TokenType.True);
                return new IntegerNode(1) {Line = line};
            }
            else if (token.Type == TokenType.False)
            {
                var line = Match(TokenType.False);
                return new IntegerNode(0) {Line = line};
            }
            else if (token.Type == TokenType.String)
            {
                var line = Match(TokenType.String);
                return new StringNode(token.Value.ToString()) {Line = line};
            }
            else if (token.Type == TokenType.LParenthesis)
            {
                var line = Match(TokenType.LParenthesis);

                var node = AssignmentExpression();
                node.Line = line;

                Match(TokenType.RParenthesis);

                return node;
            }
            else if (token.Type == TokenType.Fn)
            {
                return FunctionDefinition();
            }
            else if (token.Type == TokenType.LBrace)
            {
                return TableCreation();
            }
            else if (token.Type == TokenType.Identifier)
            {
                return Variable();
            }
            else throw new ParserException($"Unexpected primary expression token [{token}]", Scanner.State);
        }
    }
}