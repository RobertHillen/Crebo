using Crebo.Business.Views.Student;
using System.Collections.Generic;
using System.Linq;

namespace Crebo.Models
{
    public class ResultContentModel
    {
        public string ErrorMessage { get; set; }

        public string FileName { get; set; }

        public IEnumerable<StudentListView> Data { get; set; }

        public bool HasError()
        {
            return !string.IsNullOrEmpty(ErrorMessage);
        }

        public bool HasData()
        {
            return Data != null && Data.Any();
        }
    }
}