using Crude.Core.Attributes;
using Crude.Core.Models;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Crude.Demo.Wasm.ViewModel
{
    public class TestViewModel
    {
        [CrudeOrder(2)]
        public int IntegerFieldOne { get; set; } = 1;

        [CrudeOrder(1)]
        public int IntegerFieldTwo { get; set; } = 2;

        [CrudeDisable]
        public int IntegerFieldThree { get; set; } = 3;

        [CrudeIgnore]
        public int IntegerFieldFour { get; set; } = 4;

        [Display(Name = "Floating")]
        public double DoubleFieldOne { get; set; } = 1.2;

        public TestTable TableFieldOne { get; set; } = new TestTable();

        public double? DoubleFieldTwo { get; set; } = null;

        [Required]
        [StringLength(10, ErrorMessage = "Name is too long.")]
        public string StringFieldOne { get; set; } = "Hello sailor";

        public bool BoolFieldOne { get; set; } = true;

        public TestEnum Status { get; set; } = TestEnum.Active;

        public TestEnum? NullableStatus { get; set; } = null;

        public DateTime Created { get; set; } = DateTime.Now;

        public DateTimeOffset? NullableCreated { get; set; }

        public string? StringFieldTwo { get; set; } = null;

        [CrudePassword]
        public string StringFieldThree { get; set; }

        [CrudeOnSubmit("Submit")]
        private void OnSubmit(EditContext context)
        {
            Console.WriteLine(context.Validate());
            Console.WriteLine("send");
        }

        [CrudeOnButtonClick("Send It")]
        private void OnSend(EditContext context)
        {
            Console.WriteLine(context.Validate());
            Console.WriteLine("send");
        }

        [CrudeOnButtonClick("Cancel It")]
        private void OnCancel(EditContext context)
        {
            Console.WriteLine(context.Validate());
            Console.WriteLine("cancel");
        }
    }

    public class TestTable : CrudeTable<TestTableViewModel>
    {
        private static readonly IEnumerable<TestTableViewModel> Rows = new[]
        {
            new TestTableViewModel(),
            new TestTableViewModel(),
            new TestTableViewModel()
        };

        public TestTable() : base(true, true) { }

        public override ulong GetTotalElementCount()
        {
            return 27;
        }

        public override IEnumerable<TestTableViewModel> GetElements()
        {
            Console.WriteLine(UnescapedSearchTerm);
            return Rows;
        }
    }

    public class TestTableViewModel
    {
        public int TableIntegerFieldOne { get; set; } = 1;

        [Display(Name = "Id")]
        [CrudeOrder(1)]
        public int TableIntegerFieldTwo { get; set; } = 2;

        [CrudeIgnore]
        public int TableIntegerFieldThree { get; set; } = 3;

        public int TableIntegerFieldFour { get; set; } = 4;

        [Display(Name = "Description")]
        public string TableStringFieldOne { get; set; } = "Lorem Ipsum";

        [CrudeOnClick(nameof(TableStringFieldOne))]
        private void ShortenString()
        {
            TableStringFieldOne = TableStringFieldOne.Substring(0, TableStringFieldOne.Length - 1);
        }
    }

    public enum TestEnum
    {
        Active = 0,
        Passive = 1
    }
}
