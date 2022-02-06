using Crebo.Business.Models;
using Crebo.Business.Resources;
using ExcelDataReader;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace Crebo.Business.MasterData
{
    public class Duo
    {
        private List<CreboExcelModel> _listDuo;

        private const int _colCreboNummer = 0;
        private const int _colKwalificatieDossier = 12;
        private const int _colKwalificatie = 14;
        private const int _colNiveau = 15;

        public Duo()
        {
            Init();
            CreateList();
        }

        private void Init()
        {
            _listDuo = new List<CreboExcelModel>();
        }

        public void CreateList()
        {
            DataTable dt = new DataTable();
            using (var stream = new MemoryStream(CreboFiles.DUO))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    int lastCreboNr = 0;

                    dt = reader.AsDataSet().Tables[0];

                    for (int row = 0; row < dt.Rows.Count; row++)
                    {
                        if (int.TryParse(dt.Rows[row][_colNiveau].ToString(), out int niveau))
                        {
                            var duo = new CreboExcelModel()
                            {
                                CreboNummer = 0,
                                KwalificatieDossier = dt.Rows[row][_colKwalificatieDossier].ToString(),
                                Kwalificatie = dt.Rows[row][_colKwalificatie].ToString(),
                                Niveau = niveau
                            };

                            if (int.TryParse(dt.Rows[row][_colCreboNummer].ToString(), out int currentCreboNr))
                            {
                                duo.CreboNummer = currentCreboNr;
                            }
                            else
                            {
                                duo.CreboNummer = lastCreboNr;
                            }

                            _listDuo.Add(duo);

                            lastCreboNr = duo.CreboNummer;
                        }
                    }
                }
            }
        }

        public IEnumerable<CreboExcelModel> Find(int CreboNummer)
        {
            return _listDuo.Where(s => s.CreboNummer == CreboNummer).AsEnumerable();
        }
    }
}
