﻿namespace President.Domain.Cards
{
    public struct Card
    {
        private readonly int _weight;
        private readonly string _name;
        private readonly Color _color;

        public Card(int weight, string name, Color color)
        {
            _weight = weight;
            _name = name;
            _color = color;
        }

        public int Weight { get => _weight; }

        public override string ToString()
        {
            return $"{_name}-{_color}, weight {_weight}";
        }

        public enum Color
        {
            HEART,
            SPADE,
            DIAMOND,
            CLUB
        }
    }
}
