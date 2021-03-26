using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace PACS_Utils
{
    public class RsaKeysService
    {
        private const string Vocab = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private readonly CspParameters _cspp = new CspParameters();

        private readonly SecureRandom _secureRandom = new SecureRandom();

        private DataAccessService _dtb = new DataAccessService();

        private RSACryptoServiceProvider _rsa;
        //CREAR OBJETO PLANET PARA TENER TODO SU INFO EN EL CODIGO

        public string GenerateRsaKeys(string codePlanet, int planetId)
        {
            try
            {
                _dtb = new DataAccessService();
                _rsa?.Clear();

                _cspp.KeyContainerName = codePlanet;
                _rsa = new RSACryptoServiceProvider(_cspp);

                var publicKey = _rsa.ToXmlString(false);
                _rsa.PersistKeyInCsp = true;

                var sqlParams = new Dictionary<string, dynamic> {{"idplanet", planetId}};

                // GET ID OF TH PLANET FOR PLANETKEYS TABLE
                var idPlanet = _dtb.GetByQuery("SELECT * FROM dbo.PlanetKeys WHERE idPlanet = @idplanet;",
                    sqlParams);

                if (idPlanet.Tables[0].Rows.Count > 0)
                    _dtb.RunSafeQuery("DELETE FROM dbo.PlanetKeys WHERE idPlanet = @idplanet;", sqlParams);

                // INSERT INFO PUBLIC KEY OF PLANET TO BBDD
                sqlParams.Clear();
                sqlParams.Add("idplanet", planetId.ToString());
                sqlParams.Add("publickey", publicKey);

                _dtb.RunSafeQuery("INSERT INTO dbo.PlanetKeys (idPlanet, XMLKey) VALUES (@idplanet, @publickey);",
                    sqlParams);

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

                    //MessageBox.Show(xmlKey);

                    rsa.FromXmlString(xmlKey);
                    return rsa.Encrypt(dataToEncrypt, false);
                }
            }
            catch
            {
                return null;
            }
        }

        public static string DecryptCode(byte[] encryptedMessage, string planetCode)
        {
            try
            {
                var byteConverter = new UnicodeEncoding();

                var rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(GetKeyFromContainer(planetCode));

                //var dataToDecrypt = byteConverter.GetBytes(encryptedMessage);

                var decryptedMessage = byteConverter.GetString(rsa.Decrypt(encryptedMessage, false));

                return decryptedMessage;
            }
            catch
            {
                return null;
            }
        }

        private static string GetKeyFromContainer(string containerName)
        {
            var param = new CspParameters {KeyContainerName = containerName};

            var rsa = new RSACryptoServiceProvider(param);

            string key = rsa.ToXmlString(true);

            //MessageBox.Show(key);

            return key;

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

        private static List<dynamic> GetCharList()
        {
            var chars = new List<dynamic>();

            for (var i = 0; i <= 1; i++)
                chars.AddRange(Vocab.Select(letter => i == 0 ? letter : char.Parse(letter.ToString().ToLower()))
                    .Cast<dynamic>());

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

        public static string GenerateRandomLetters(int length)
        {
            var strBuild = new StringBuilder();
            var random = new Random();

            for (var i = 0; i < length; i++)
            {
                var flt = random.NextDouble();
                var shift = Convert.ToInt32(Math.Floor(25 * flt));
                var letter = Convert.ToChar(shift + 65);
                strBuild.Append(letter);
            }

            return strBuild.ToString();
        }
    }
}