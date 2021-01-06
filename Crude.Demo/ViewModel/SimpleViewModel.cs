using Crude.Models;
using Crude.Models.Attributes;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Crude.Demo.ViewModel
{
    public class SimpleViewModel
    {
        [CrudeOrder(2)]
        public int IntegerFieldOne { get; set; } = 1;

        [CrudeOrder(1)]
        public int IntegerFieldTwo { get; set; } = 2;

        public int IntegerFieldThree { get; set; } = 3;

        [CrudeIgnore]
        public int IntegerFieldFour { get; set; } = 4;

        [CrudeName("Floating")]
        public double DoubleFieldOne { get; set; } = 1.2;

        public SimpleTable TableFieldOne { get; set; } = new SimpleTable();

        public double? DoubleFieldTwo { get; set; } = null;

        [Required]
        [StringLength(10, ErrorMessage = "Name is too long.")]
        public string StringFieldOne { get; set; } = "Hello sailor";

        public bool BoolFieldOne { get; set; } = true;

        public SimpleEnum Status { get; set; } = SimpleEnum.Active;

        public SimpleEnum? NullableStatus { get; set; } = null;

        public DateTime Created { get; set; } = DateTime.Now;

        public DateTimeOffset? NullableCreated { get; set; }

        public string? StringFieldTwo { get; set; } = null;

        [CrudeOnSubmit("Send It")]
        private void OnSubmit(EditContext context)
        {
            Console.WriteLine("asd");
        }
    }

    public class SimpleTable : CrudeTable<SimpleTableViewModel>
    {
        private static readonly IEnumerable<SimpleTableViewModel> Rows = new[]
        {
            new SimpleTableViewModel(),
            new SimpleTableViewModel(),
            new SimpleTableViewModel()
        };

        public override IEnumerable<SimpleTableViewModel> GetElements(int index, int size)
        {
            return Rows;
        }
    }

    public class SimpleTableViewModel
    {
        public int TableIntegerFieldOne { get; set; } = 1;

        [CrudeName("Id")]
        [CrudeOrder(1)]
        public int TableIntegerFieldTwo { get; set; } = 2;

        [CrudeIgnore]
        public int TableIntegerFieldThree { get; set; } = 3;

        public int TableIntegerFieldFour { get; set; } = 4;

        [CrudeName("Description")]
        public string TableStringFieldOne { get; set; } = "Lorem Ipsum";

        [CrudeOnClick(nameof(TableStringFieldOne))]
        private void ShortenString()
        {
            TableStringFieldOne = TableStringFieldOne.Substring(0, TableStringFieldOne.Length - 1);
        }
    }

    public enum SimpleEnum
    {
        Active = 0,
        Passive = 1
    }
}
