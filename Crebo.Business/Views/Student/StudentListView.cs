namespace Crebo.Business.Views.Student
{
    public class StudentListView
    {
        public string FirstName { get; set; }

        public string Prefix { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public int CreboNummer { get; set; }

        public string KwalificatieDossier { get; set; }

        public string Kwalificatie { get; set; }

        public int? Niveau { get; set; }

        public string Bron { get; set; }

        public string FullName()
        {
            return $"{LastName}, {FirstName} {Prefix}";
        }
    }
}
