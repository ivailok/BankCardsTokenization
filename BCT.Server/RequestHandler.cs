using BCT.Data;
using BCT.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BCT.Server
{
    public class RequestHandler : IDisposable
    {
        private readonly Socket RequestSocket;
        private readonly NetworkStream SocketStream;
        private readonly BinaryReader Reader;
        private readonly BinaryWriter Writer;
        private readonly BinaryFormatter Formatter;
        private readonly UsersService UsersService;
        private readonly TokenizationService TokenizationService;

        public RequestHandler(Socket requestSocket)
        {
            this.RequestSocket = requestSocket;
            this.SocketStream = new NetworkStream(this.RequestSocket);
            this.Reader = new BinaryReader(this.SocketStream);
            this.Writer = new BinaryWriter(this.SocketStream);
            this.Formatter = new BinaryFormatter();
            this.UsersService = new UsersService();
            this.TokenizationService = new TokenizationService();

    }

    public void Execute()
        {
            string username;
            if (Authorize(out username))
            {
                while (true)
                {
                    Request request = this.Formatter.Deserialize(this.SocketStream) as Request;

                    Response response = new Response();
                    try
                    {
                        switch (request.RequestType)
                        {
                            case RequestType.Logout:
                                {
                                    response.Message = "Successfully logged out.";
                                    break;
                                }
                            case RequestType.RegisterToken:
                                {
                                    string token;
                                    if (this.TokenizationService.RegisterToken(request.Data as string, out token))
                                    {
                                        response.Data = token;
                                        response.Message = "Successfully registered token.";
                                    }
                                    else
                                    {
                                        throw new ArgumentException("Invalid card number.");
                                    }
                                    break;
                                }
                            case RequestType.GetCardNumber:
                                {
                                    string cardNumber = this.TokenizationService.GetCardNumber(request.Data as string);
                                    response.Data = cardNumber;
                                    response.Message = "Successfully taken card number.";
                                    break;
                                }
                            case RequestType.Terminate:
                                {
                                    response.Message = "Successfully terminated.";
                                    break;
                                }
                            default:
                                {
                                    throw new InvalidOperationException(
                                        string.Format("Operation {0} is not supported.", request.RequestType));
                                }
                        }
                        response.ResponseType = ResponseType.Success;
                    }
                    catch (Exception e)
                    {
                        response.ResponseType = ResponseType.Error;
                        response.Message = e.Message;
                    }

                    this.Formatter.Serialize(this.SocketStream, response);

                    if (request.RequestType == RequestType.Logout)
                    {
                        if (!Authorize(out username))
                        {
                            break;
                        }
                    }

                    if (request.RequestType == RequestType.Terminate)
                    {
                        break;
                    }
                }
            }

            this.Dispose();
        }

        public bool Authorize(out string username)
        {
            username = null;
            while (true)
            {
                Request request = this.Formatter.Deserialize(this.SocketStream) as Request;

                Response response = new Response();
                try
                {
                    switch (request.RequestType)
                    {
                        case RequestType.Login:
                            {
                                var login = request.Data as Login;
                                this.UsersService.Login(login);
                                username = login.Username;
                                response.Message = "Successfully logged!";
                                break;
                            }
                        case RequestType.Register:
                            {
                                var reg = request.Data as Register;
                                this.UsersService.Register(reg);
                                username = reg.Username;
                                response.Message = "Successfully registered!";
                                break;
                            }
                        case RequestType.Terminate:
                            {
                                break;
                            }

                    }
                    response.ResponseType = ResponseType.Success;
                }
                catch (Exception e)
                {
                    response.ResponseType = ResponseType.Error;
                    response.Message = e.Message;
                }

                this.Formatter.Serialize(this.SocketStream, response);

                if (username != null)
                {
                    break;
                }

                if (request.RequestType == RequestType.Terminate)
                {
                    return false;
                }
            }

            return true;
        }

        public void Dispose()
        {
            this.Reader.Dispose();
            this.Writer.Dispose();
            this.SocketStream.Dispose();
            this.RequestSocket.Dispose();
        }
    }
}
