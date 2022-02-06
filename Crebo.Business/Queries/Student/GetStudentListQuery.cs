using Crebo.Base.Interfaces;
using Crebo.Business.MasterData;
using Crebo.Business.Models;
using Crebo.Business.Views.Student;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.IO;
using System.Linq;

namespace Crebo.Business.Queries.Student
{
    public sealed class GetStudentListQuery : IQuery<IEnumerable<StudentListView>>
    {
        [Required]
        public Stream csv { get; set; }

        [Required]
        public int length { get; set; }
    }

    public sealed class GetStudentListQueryHandler : IQueryHandler<GetStudentListQuery, IEnumerable<StudentListView>>
    {
        private string[] _csvContent;
        private Sbb _sbbData;
        private Duo _duoData;

        private const int _colFirstName = 0;
        private const int _colPrefix = 1;
        private const int _colLastName = 2;
        private const int _colEmail = 3;
        private const int _colCreboNummer = 4;

        public IEnumerable<StudentListView> Handle(GetStudentListQuery query)
        {
            Init(query);

            bool.TryParse(ConfigurationManager.AppSettings["useSBB"], out bool useSBB);
            bool.TryParse(ConfigurationManager.AppSettings["useDUO"], out bool useDUO);

            var result = new List<StudentListView>();

            foreach (var item in _csvContent)
            {
                var data = item.Split(';');
                if (data.Length == 5)
                {
                    if (int.TryParse(data[_colCreboNummer], out int crebo))
                    {
                        List<StudentListView> sbbList = new List<StudentListView>();
                        List<StudentListView> duoList = new List<StudentListView>();
                        if (useSBB)
                        {
                            var sbb = _sbbData.Find(crebo);
                            sbbList = createStudentList(crebo, sbb, "SBB", data);
                        }
                        if (useDUO && !sbbList.Any())
                        {
                            var duo = _duoData.Find(crebo);
                            duoList = createStudentList(crebo, duo, "DUO", data);
                        }

                        if (sbbList.Any())
                        {
                            result.AddRange(sbbList);
                        }
                        else if (duoList.Any())
                        {
                            result.AddRange(duoList);
                        }
                        else
                        {
                            result.Add(NewStudent(crebo, null, null, data));
                        }
                    }
                }
            }

            return result;
        }

        private void Init(GetStudentListQuery query)
        {
            _sbbData = new Sbb();
            _duoData = new Duo();

            using (BinaryReader b = new BinaryReader(query.csv))
            {
                byte[] binData = b.ReadBytes(query.length);
                var result = System.Text.Encoding.UTF8.GetString(binData);

                _csvContent = result.Replace("\r\n", "\n").Split("\n".ToCharArray());
            }
        }

        private List<StudentListView> createStudentList(int crebo, IEnumerable<CreboExcelModel> list, string bron, string[] data)
        {
            var result = new List<StudentListView>();
            if (list.Any())
            {
                foreach (var item in list)
                {
                    result.Add(NewStudent(crebo, item, bron, data));
                }
            }

            return result;
        }

        private StudentListView NewStudent(int creboNr, CreboExcelModel model, string bron, string[] data)
        {
            return new StudentListView()
            {
                FirstName = data[_colFirstName],
                Prefix = data[_colPrefix],
                LastName = data[_colLastName],
                Email = data[_colEmail],
                CreboNummer = creboNr,
                KwalificatieDossier = model?.KwalificatieDossier,
                Kwalificatie = model?.Kwalificatie,
                Niveau = model?.Niveau,
                Bron = bron
            };
        }
    }
}
