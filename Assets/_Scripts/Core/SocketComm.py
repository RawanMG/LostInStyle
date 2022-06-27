import numpy as np
import socketserver
import time
from get_stress import load_model, compute_stress_model

class SocketCommHandler(socketserver.BaseRequestHandler):
    def handle(self):
        model = load_model()
        while True:
            rcv_str = "$"

            while True:
                # self.request is the TCP socket connected to the client
                data = self.request.recv(1024)
            #    print("{} wrote:".format(self.client_address[0]))
                rcv_str = rcv_str + data.decode()
                pos_ed = rcv_str.rfind('$')
                if pos_ed>0:
                    tmp = rcv_str[0:pos_ed]
                    pos_st = tmp.rfind('$')
                    if pos_st <0:
                        continue
                    str = tmp[pos_st+1:]
#                    print(str)
#                    msg = "%f" % get_stress(str)
                    msg = "%f" % compute_stress_model(str,model)
                    self.request.sendall(msg.encode())
                    rcv_str = "$"



def get_stress(dat):
    print(dat)
#    time.sleep(1/60)
    return np.random.rand()


if __name__ == "__main__":
    HOST, PORT = "localhost", 9998

    print("start")
    # Create the server, binding to localhost on port 9998
    hdlr = SocketCommHandler
    server = socketserver.TCPServer((HOST, PORT), hdlr)

    # Activate the server; this will keep running until you
    # interrupt the program with Ctrl-C
    server.serve_forever()