using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PACS_Utils
{
    public class RSAKeysService
    {
        RSACryptoServiceProvider RSA;
        CspParameters cspp;
        DataAccessService dtb;
        //CREAR OBJETO PLANET PARA TENER TODO SU INFO EN EL CODIGO
    
        public string GenerateRsaKeys (string PlanetCode) {
            try
            {
                dtb = new DataAccessService();
                RSA.Clear();

                cspp.KeyContainerName = PlanetCode;
                RSA = new RSACryptoServiceProvider(cspp);

                string publicKey = RSA.ToXmlString(false);
                RSA.PersistKeyInCsp = true;

                // GET ID OF TH PLANET FOR PLANETKEYS TABLE
                string sql = "SELECT idPlanet FROM dbo.Planets WHERE CodePlanet = '"+PlanetCode+"';";
                DataSet idPlanet = dtb.GetByQuery(sql);
                string id = idPlanet.Tables[0].Rows[0].ItemArray.GetValue(0).ToString();

                // INSERT INFO PUBLIC KEY OF PLANET TO BBDD
                string sqlInsert = "INSERT INTO dbo.PlanetKeys (idPlanet, XMLKey) VALUES (" + id + ", '" + publicKey + "');";
                dtb.RunQuery(sqlInsert);

                return "[SYSTEM] - Keys Created Succesfully!";
            }
            catch
            {
                return "[ERROR] - Failed to create RSAKeys!";
            }
        }

        public byte [] EncryptedCode(String MessageToEncrypt, string idPlanet) {
            
            try
            {
                UnicodeEncoding ByteConverter = new UnicodeEncoding();
                byte[] dataToEncrypt = ByteConverter.GetBytes(MessageToEncrypt);

                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    string sql = "SELECT XMLKey FROM dbo.PlanetKeys WHERE idPlanet = " + idPlanet + ";";
                    DataSet PlanetKey = dtb.GetByQuery(sql);
                    string xmlKey = PlanetKey.Tables[0].Rows[0].ItemArray.GetValue(0).ToString();

                    RSA.FromXmlString(xmlKey);
                    return RSA.Encrypt(dataToEncrypt, false);
                }
            } catch
            {
                return null;
            }
        }

        public string DencryptedCode (byte[] EncryptMessage, String PlanetCode) {
            try
            {
                UnicodeEncoding ByteConverter = new UnicodeEncoding();

                var rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(GetKeyFromContainer(PlanetCode));

                string DecryptedMessage = ByteConverter.GetString(rsa.Decrypt(EncryptMessage, false));

                return DecryptedMessage;
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
    }
}
