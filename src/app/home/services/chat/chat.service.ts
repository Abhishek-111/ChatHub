import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { Observable } from 'rxjs';
import { IChat } from 'src/app/core/Interfaces/chat/IChat';
import { IDate } from 'src/app/core/Interfaces/date/IDate';
import { IFile } from 'src/app/core/Interfaces/file/IFile';
import { IGroup } from 'src/app/core/Interfaces/group/IGroup';
import { IMessage } from 'src/app/core/Interfaces/message/IMessage';
import { ConfigService } from 'src/app/core/services/config/config.service';

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  private chatConnection!:HubConnection;
  public availableUsers:string[]=[];
  public privateMessages: IChat[]=[];
  private privateChatInitiated: boolean = false

  /**
   * Creates an instance of chat service
   * @param configService 
   * @param http 
   */
  constructor(private configService:ConfigService,private http:HttpClient) {}

  /**
   * Creates a signalr connection 
   */
  public createConnection(){
    this.chatConnection=new HubConnectionBuilder().withUrl(this.configService.getApiUrl()+"/hubs/chat").build();

    this.chatConnection.on('UserConnected',()=>{
      this.addUserConnectionId(this.getNameFromStorage());
    })
    this.chatConnection.on('OnlineChat', (availableUsers) => {
      this.availableUsers=[...availableUsers];
    });


    this.chatConnection.on('OpenPrivateChat',(message:IChat) => {
      this.privateMessages = [...this.privateMessages, message];
      this.privateChatInitiated=true;
    });

    this.chatConnection.on('NewPrivateMessage',(message:IChat) => {
      this.privateMessages=[...this.privateMessages,message];
      console.log('hello');
    });

    this.chatConnection.start().then(()=>console.log("Connection Started"))
    .catch(err=>{console.log(err)});
  }

  /**
   * Adds id of the user 
   * @param name 
   * @returns ;
   */
  public async addUserConnectionId(name:string){
    return this.chatConnection?.invoke('AddUserConnectionId',name)
    .catch(error => console.log(error));
  }

  /**
   * Sends message to a specific user
   * @param message 
   * @returns ;
   */
  public async sendPrivateMessage(message:IChat){
    if(!this.privateChatInitiated){
      this.privateChatInitiated = true;
      return this.chatConnection?.invoke('CreatePrivateChat',message).then(() => {
        this.privateMessages = [...this.privateMessages,message];
        console.log(this.privateMessages);
      })
      .catch(error => console.log(error));
    }
    else{
      return this.chatConnection?.invoke('ReceivePrivateMessage',message)
    .catch(error => console.log(error));
    }
  }

  /**
   * Fetches the chat history of two users
   * @param group 
   * @returns chat
   */
  public getPreviousChatHistory(group:IGroup):Observable<IChat[]>{
    return this.http.post<IChat[]>(this.configService.getApiUrl()+'/api/chat/previous-chat',group);
  }

  /**
   * Fetches the top 2 dates of chat between tow users
   * @param date 
   * @returns date
   */
  public getTop2Dates(date:IDate):Observable<Date[]>{
    return this.http.post<Date[]>(this.configService.getApiUrl()+'/api/chat/dates',date);
  }

  /**
   * Fetches the user name from storage
   * @returns string
   */
  public getNameFromStorage(){
    return sessionStorage.getItem('userName')||'';
  }

  /**
 * 
 * @param id 
 * @returns IFile
 */
  public getFileById(id: string): Observable<IFile> {
    return this.http.get(
      this.configService.getApiUrl() + '/api/chat/file-download/' + id
    );
  }

}

