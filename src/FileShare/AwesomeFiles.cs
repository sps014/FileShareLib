using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System;


namespace AwesomeFileTransfer
{
    public class AwesomeFileSender
    {
        IPAddress _address;
        TcpClient _client;
        TcpClient _clientInfo;


        int _port;

        NetworkStream streamInfo;

        public AwesomeFileSender(IPAddress address,int port)
        {
            _address = address;
            _port = port;
        }
        public AwesomeFileSender(string address,int port)
        {
            _address = IPAddress.Parse(address);
            _port = port;
        }

        bool Connect()
        {
            try
            {
                if (_client != null)
                    _client.Close();

                _client = new TcpClient();
                _clientInfo = new TcpClient();

                _client.Connect(_address, _port);
                _clientInfo.Connect(_address, _port + 1);

                streamInfo = _clientInfo.GetStream();

                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }
        public bool sendFile(string file)
        {

            try
            {
                Connect();
                sendInfo(file);
                sendProcessing(file);
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }
        void sendInfo(string file)
        {
             
            FileInfo _info = new FileInfo(file);
            byte[] fileSize = Encoding.UTF8.GetBytes(_info.Length.ToString()+@"..@.."+Path.GetFileName(file));
            streamInfo.Write(fileSize, 0, fileSize.Length);
        }
        void sendProcessing(string file)
        {
            _client.Client.SendFile(file);
        }
        public delegate void SendCompleteHandler(object sender);
        public event SendCompleteHandler SendCompleted;
    }

    public class AwesomeFileReciever
    {
        #region global var and contructor

        public TcpListener _listener,_listenerInfo;
        TcpClient _client;
        TcpClient _clientInfo;

        long _size;

        NetworkStream streamInfo;
        public AwesomeFileReciever(IPAddress address,int port)
        {
            _listener = new TcpListener(address, port);
            _listenerInfo = new TcpListener(address, port + 1);

        }
        #endregion

        public bool RecieveFile(string directory=null)
        {
            try
            {
                if (RecieveStarted != null)
                    RecieveStarted(this);

                if (directory==null)
                {
                    directory = AppDomain.CurrentDomain.BaseDirectory;
                }

                _listener.Start();
                _listenerInfo.Start();

                _client = _listener.AcceptTcpClient();
                _clientInfo = _listenerInfo.AcceptTcpClient();

                streamInfo = _clientInfo.GetStream();

                string fullInfo = GetFullInfo();
                string file = directory + GetFileName(fullInfo);

                if ((_size = GetFileSize(fullInfo)) != 0)
                {
                    ReceiveLargeFile(_client.Client, file);
                    return true;
                }
                return false;
            }
            catch(Exception c)
            {
                return false;
            }

        }

        private void ReceiveLargeFile(Socket socket,string file)
        {

            byte[] data = new byte[_size];

            int total = 0; 
            int dataleft = (int)_size;
            FileStream fstream = new FileStream(file, FileMode.Create);
            ProgressArgs pr = new ProgressArgs();
            pr.totalBytes = _size;
            pr.filename = file;
            int  recv = 0;
            while (total < _size)
            {

                 recv = socket.Receive(data, total,dataleft, SocketFlags.None);
                 pr.bytesRecieved = total;
                 pr.speed = recv;
                 fstream.Write(data, total, recv);
                  if(ProgressChange!=null)
                    ProgressChange(this,pr);
              

                 if (recv == 0)
                {
                    data = null;
                    break;
                }
                total += recv;
                dataleft -= recv; 
            }
            fstream.Close();
            if (RecievedFile != null)
                RecievedFile(this);
            return; 
        }


        #region Information about file gathering
        string GetFullInfo()
        {
            byte[] fileSize = new byte[1024];
            streamInfo.Read(fileSize, 0, fileSize.Length);
            return Encoding.UTF8.GetString(fileSize);
        }
     
        int GetFileSize(string strSize)
        {
           
            int i = strSize.IndexOf("..@..");
            strSize = strSize.Substring(0, i);
            try
            {
                return int.Parse(strSize);
            }
            catch
            {
                return 0;
            }
        }
        string GetFileName(string str)
        {
            int i = str.IndexOf("..@..") +5;
            str= str.Substring(i, str.Length - i);
            int j = str.IndexOf('\0');
            return str.Substring(0,j);
        }

        #endregion


        #region events
        public delegate void RecieveStartedHandler(object sender);
        public event RecieveStartedHandler RecieveStarted;

        public delegate void ProgressChangeHandler(object sender,ProgressArgs e);
        public event ProgressChangeHandler ProgressChange;
        public class ProgressArgs : EventArgs
        {
            public string filename;
            public long bytesRecieved;
            public long totalBytes;
            public long speed;
        }

        public delegate void RecievedFileHandler(object sender);
        public event RecievedFileHandler RecievedFile;
        #endregion
    }


}
