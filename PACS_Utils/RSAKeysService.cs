using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace PACS_Utils
{
    public class RsaKeysService
    {
        private const string Vocab = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        private readonly SecureRandom _secureRandom = new SecureRandom();
        private CspParameters _cspp = new CspParameters();

        private DataAccessService _dtb = new DataAccessService();

        private RSACryptoServiceProvider _rsa;
        //CREAR OBJETO PLANET PARA TENER TODO SU INFO EN EL CODIGO

        public string GenerateRsaKeys(string planetCode)
        {
            try
            {
                _dtb = new DataAccessService();
                _rsa.Clear();

                _cspp.KeyContainerName = planetCode;
                _rsa = new RSACryptoServiceProvider(_cspp);

                var publicKey = _rsa.ToXmlString(false);
                _rsa.PersistKeyInCsp = true;

                // GET ID OF TH PLANET FOR PLANETKEYS TABLE
                var sql = $@"SELECT idPlanet FROM dbo.Planets WHERE CodePlanet = '{planetCode}';";
                var idPlanet = _dtb.GetByQuery(sql);
                var id = idPlanet.Tables[0].Rows[0].ItemArray.GetValue(0).ToString();

                // INSERT INFO PUBLIC KEY OF PLANET TO BBDD
                var sqlInsert = $@"INSERT INTO dbo.PlanetKeys (idPlanet, XMLKey) VALUES ({id}, '{publicKey}');";
                _dtb.RunQuery(sqlInsert);

                return "[SYSTEM] - Keys Created Succesfully!";
            }
            catch
            {
                return "[ERROR] - Failed to create RSAKeys!";
            }
        }

        public byte[] EncryptedCode(string messageToEncrypt, string idPlanet)
        {
            try
            {
                var byteConverter = new UnicodeEncoding();
                var dataToEncrypt = byteConverter.GetBytes(messageToEncrypt);

                using (var rsa = new RSACryptoServiceProvider())
                {
                    var sql = $@"SELECT XMLKey FROM dbo.PlanetKeys WHERE idPlanet = {idPlanet};";
                    var planetKey = _dtb.GetByQuery(sql);
                    var xmlKey = planetKey.Tables[0].Rows[0].ItemArray.GetValue(0).ToString();

                    rsa.FromXmlString(xmlKey);
                    return rsa.Encrypt(dataToEncrypt, false);
                }
            }
            catch
            {
                return null;
            }
        }

        public string DencryptedCode(byte[] encryptMessage, string planetCode)
        {
            try
            {
                var byteConverter = new UnicodeEncoding();

                var rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(GetKeyFromContainer(planetCode));

                var decryptedMessage = byteConverter.GetString(rsa.Decrypt(encryptMessage, false));

                return decryptedMessage;
            }
            catch
            {
                return null;
            }
        }

        private string GetKeyFromContainer(string containerName)
        {
            var param = new CspParameters();

            param.KeyContainerName = containerName;

            var rsa = new RSACryptoServiceProvider(param);

            return rsa.ToXmlString(true);
        }

        public string GenerateKey(int length)
        {
            var key = "";
            var chars = GetCharList();

            for (var i = 0; i < length; i++)
            {
                var pos = _secureRandom.Next(chars.Count);
                key += chars[pos];
                chars.Remove(pos);
            }

            return key;
        }

        private List<dynamic> GetCharList()
        {
            var chars = new List<dynamic>();

            for (var i = 0; i <= 1; i++)
                foreach (var letter in Vocab)
                    chars.Add(i == 0 ? letter : char.Parse(letter.ToString().ToLower()));

            for (var i = 0; i <= 9; i++) chars.Add(i);

            return chars;
        }

        public Dictionary<char, string> GenerateEncryptedChars()
        {
            var charsList = new Dictionary<char, string>();

            foreach (var letter in Vocab)
            {
                string encryptedNumbers;
                do
                {
                    encryptedNumbers = _secureRandom.Next(999).ToString("D3");
                } while (charsList.ContainsValue(encryptedNumbers));

                charsList.Add(letter, encryptedNumbers);
            }

            return charsList;
        }
    }
}