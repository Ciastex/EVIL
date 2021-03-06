﻿namespace EVIL.Grammar.AST.Nodes
{
    public class DecrementationNode : AstNode
    {
        public AstNode Target { get; }
        public bool IsPrefix { get; }

        public DecrementationNode(AstNode target, bool isPrefix)
        {
            target.Parent = this;
            
            Target = target;
            IsPrefix = isPrefix;
        }
    }
}
