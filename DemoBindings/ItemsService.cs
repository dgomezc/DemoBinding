using System.Collections.Generic;

namespace DemoBindings
{
    public static class ItemsService
    {
        public static IEnumerable<Item> GetItems()
        {
            return new List<Item>
            {
                new Item("Item 1"),
                new Item("Item 2"),
                new Item("Item 3"),
                new Item("Item 4"),
                new Item("Item 5")
            };
        }
    }
}
