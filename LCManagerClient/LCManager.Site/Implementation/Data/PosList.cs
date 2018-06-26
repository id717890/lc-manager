namespace LC_Manager.Implementation.Data
{
    using System;
    using System.Collections.Generic;

    public class OperatorPosList
    {
        public Int16 Id { get; set; }
        public string Caption { get; set; }
        public Int16 Operator { get; set; }
        public string Text { get; set; }
        public string HtmlForDelete { get; set; }
        public IEnumerable<Pos> Poses { get; set; }
        public string details { get; set; }

        public OperatorPosList()
        {
            Poses = new List<Pos>();
        }
    }
}