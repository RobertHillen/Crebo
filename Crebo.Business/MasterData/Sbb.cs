using Crebo.Business.Models;
using Crebo.Business.Resources;
using ExcelDataReader;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace Crebo.Business.MasterData
{
    public class Sbb
    {
        private List<CreboExcelModel> _listSbb;

        private const int _colCreboNummer = 0;
        private const int _colKwalificatieDossier = 2;
        private const int _colKwalificatie = 4;
        private const int _colNiveau = 5;

        public Sbb()
        {
            Init();
            CreateList();
        }

        private void Init()
        {
            _listSbb = new List<CreboExcelModel>();
        }

        public void CreateList()
        {
            DataTable dt = new DataTable();
            using (var stream = new MemoryStream(CreboFiles.SBB))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    int lastCreboNr = 0;

                    dt = reader.AsDataSet().Tables[0];

                    for (int row = 0; row < dt.Rows.Count; row++)
                    {
                        if (int.TryParse(dt.Rows[row][_colNiveau].ToString(), out int niveau))
                        {
                            var sbb = new CreboExcelModel()
                            {
                                CreboNummer = 0,
                                KwalificatieDossier = dt.Rows[row][_colKwalificatieDossier].ToString(),
                                Kwalificatie = dt.Rows[row][_colKwalificatie].ToString(),
                                Niveau = niveau
                            };

                            if (int.TryParse(dt.Rows[row][_colCreboNummer].ToString(), out int currentCreboNr))
                            {
                                sbb.CreboNummer = currentCreboNr;
                            }
                            else
                            {
                                sbb.CreboNummer = lastCreboNr;
                            }

                            _listSbb.Add(sbb);

                            lastCreboNr = sbb.CreboNummer;
                        }
                    }
                }
            }
        }

        public IEnumerable<CreboExcelModel> Find(int CreboNummer)
        {
            return _listSbb.Where(s => s.CreboNummer == CreboNummer).AsEnumerable();
        }
    }
}
