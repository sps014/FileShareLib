# Awesome-File-Transfer-Alpha-
TCP File Transfer libary using c# the libary.
Created own chunking method to optimize the sending and recieving process

Use these 2 classes
```
AwesomeFileSender 
```
#### constructor , public functions ,events

| function             |      type    | params  decriptions |
| :--------:           |     :-:      | :-:         |  
| AwesomeFileSender    | constructor  | recieves ipaddress(IPAddress) ,port(int) of the system you want to connect  |
| AwesomeFileSender    | constructor  | recieves ipaddress(string) ,port(int) of the system you want to connect     |
| sendFile             |      bool    | sends file to remote reciever , needs path of file to be sent (string).     |
| SendCompleted        |   void event | triggered when operation is successfully completed                          |

```
AwesomeFileReciever
```
These classes present easy to use methods for complete transmission process
