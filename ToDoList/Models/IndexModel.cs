﻿using ToDoList.DBmodels;

namespace ToDoList.Models
{
    public class IndexModel
    {
        public List<Tasks>? Tasks { get; set; }

        public List<Category>? Categories{get; set;}

        public TaskValidation? TaskValidation { get; set; }

    }
}
