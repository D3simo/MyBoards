﻿using MyBoards.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MyBoards.Dto
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; }
        public int TotalPages { get; set; }
        public int ItemsFrom { get; set; }
        public int ItemsTo { get; set; }
        public int TotalItemsCount { get; set; }

        public PagedResult(List<T> items, int totalCount, int pageSize, int pageNumber)
        {
            Items = items;
            TotalItemsCount = totalCount;
            ItemsFrom = (pageNumber - 1) * pageSize + 1; // Start from the next item
            ItemsTo = ItemsFrom + pageSize - 1;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize); // 12 / 5 = 2.4 -> 3
        }

        public PagedResult()
        {
        }
    }
}
