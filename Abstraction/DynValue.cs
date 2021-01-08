﻿using System.Globalization;

namespace EVIL.Abstraction
{
    public class DynValue
    {
        public static readonly DynValue Zero = new(0);

        private double _numberValue;
        private string _stringValue;
        private Table _tableValue;
        private ScriptFunction _functionValue;

        public DynValueType Type { get; private set; }

        public double Number
        {
            get
            {
                if (Type != DynValueType.Number)
                    throw new InvalidDynValueTypeException("This value is not a number.", DynValueType.Number, Type);

                return _numberValue;
            }
        }

        public string String
        {
            get
            {
                if (Type != DynValueType.String)
                    throw new InvalidDynValueTypeException("This value is not a string.", DynValueType.String, Type);

                return _stringValue;
            }
        }

        public Table Table
        {
            get
            {
                if (Type != DynValueType.Table)
                    throw new InvalidDynValueTypeException($"This value is not a table.", DynValueType.Table, Type);

                return _tableValue;
            }
        }

        public ScriptFunction Function
        {
            get
            {
                if (Type != DynValueType.Function)
                    throw new InvalidDynValueTypeException($"This value is not a function.", DynValueType.Function,
                        Type);

                return _functionValue;
            }
        }

        private DynValue(DynValue dynValue)
        {
            CopyFrom(dynValue);
        }

        public DynValue(double value)
        {
            Type = DynValueType.Number;
            _numberValue = value;
        }

        public DynValue(string value)
        {
            Type = DynValueType.String;
            _stringValue = value;
        }

        public DynValue(Table value)
        {
            Type = DynValueType.Table;
            _tableValue = value;
        }

        public DynValue(ScriptFunction function)
        {
            Type = DynValueType.Function;
            _functionValue = function;
        }

        public DynValue AsNumber()
        {
            if (Type == DynValueType.Number)
                return new DynValue(_numberValue);

            var success = double.TryParse(_stringValue, out var result);

            if (!success)
                throw new DynValueConversionException($"Cannot convert string value '{_stringValue}' to number.");

            return new DynValue(result);
        }

        public DynValue AsString()
        {
            if (Type == DynValueType.String)
                return new DynValue(_stringValue);
            else if (Type == DynValueType.Table)
                return new DynValue($"Table (count: {_tableValue.Count})");
            else if (Type == DynValueType.Function)
                return new DynValue($"fn({_functionValue.ParameterNames.Count})");
            return new DynValue(_numberValue.ToString(CultureInfo.InvariantCulture));
        }

        public DynValue AsTable()
        {
            if (Type == DynValueType.String)
                return new DynValue(Table.FromString(_stringValue));
            else if (Type == DynValueType.Table)
                return new DynValue(_tableValue);
            else throw new DynValueConversionException($"Cannot convert a value type '{Type}' to a table.");
        }

        public DynValue Copy()
        {
            return new(this);
        }

        public void CopyFrom(DynValue dynValue)
        {
            Type = dynValue.Type;

            switch (dynValue.Type)
            {
                case DynValueType.Number:
                    _numberValue = dynValue.Number;
                    break;

                case DynValueType.String:
                    _stringValue = dynValue.String;
                    break;

                case DynValueType.Table:
                    _tableValue = dynValue.Table;
                    break;
                
                case DynValueType.Function:
                    _functionValue = dynValue.Function;
                    break;
            }
        }
    }
}