# Awesome-File-Transfer-Alpha-
TCP File Transfer libary using c# the libary.
Created own chunking method to optimize the sending and recieving process and progress reporting system.

## Usage
#### constructor , public functions ,events

```
AwesomeFileSender 
```
| function             |      type    |   decriptions |
| :--------:           |     :-:      | :-:         |  
| AwesomeFileSender    | constructor  | recieves ipaddress(IPAddress) ,port(int) of the system you want to connect  |
| AwesomeFileSender    | constructor  | recieves ipaddress(string) ,port(int) of the system you want to connect     |
| sendFile             |      bool    | sends file to remote reciever , needs path of file to be sent (string).     |
| SendCompleted        |   void event | triggered when operation is successfully completed                          |



```
AwesomeFileReciever
```
| function               |      type    | params  decriptions |
| :--------:             |     :-:      | :-:         |  
| AwesomeFileReciever    | constructor  | recieves ipaddress(IPAddress) ,port(int) of the system you want to connect  |
| RecieveFile             |      bool    | recieve a file from a remote sender , optional directory path of file to be stored (string).  |
| RecieveStarted        |   void event | triggered when operation is successfully started                          |
|ProgressChange        | void event| triggers when chunk of file is recieved  , use ProgressArgs for more obtainging speed info.|
|RecievedFile          |void event |triggers when file is recieved successfully  |

These classes present easy to use methods for complete transmission process.
