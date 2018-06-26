namespace LC_Manager.Implementation.Data
{
    public class Good
    {
        /// <summary>
        /// Идентификатор товара
        /// </summary>
        public short Id { get; set; }

        /// <summary>
        /// код товара
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// наименование
        /// </summary>
        public string Name { get; set; }
    }
}