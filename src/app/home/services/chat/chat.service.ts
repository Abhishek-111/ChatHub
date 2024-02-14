import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { Observable } from 'rxjs';
import { IChat } from 'src/app/core/Interfaces/chat/IChat';
import { IDate } from 'src/app/core/Interfaces/date/IDate';
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
  constructor(private configService:ConfigService,private http:HttpClient) {}

  // public addMessage(message:IChat){
  //   return this.http.post(this.configService.getApiUrl()+'/addMessage',message);
  // }
  public createConnection(){
    this.chatConnection=new HubConnectionBuilder().withUrl(this.configService.getApiUrl()+"/hubs/chat").build();

    // this.chatConnection.start().catch(error=>{
    //   console.log(error);
    // });

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

    // this.chatConnection.on('ClosePrivateChat',() => {
    //   console.log('msg');
    //   this.privateChatInitiated=false;
    //   this.privateMessages=[];
    // });

    this.chatConnection.start().then(()=>console.log("Connection Started"))
    .catch(err=>{console.log(err)});
  }

  public async addUserConnectionId(name:string){
    return this.chatConnection?.invoke('AddUserConnectionId',name)
    .catch(error => console.log(error));
  }
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

  // public async closePrivateChatMessage(group:IGroup){
  //   //this.privateMessages=[];
  //   return this.chatConnection?.invoke('RemovePrivateChat')
  //   .catch(error => console.log(error));
  // }
  public getPreviousChatHistory(group:IGroup):Observable<IMessage[]>{
    return this.http.post<IMessage[]>(this.configService.getApiUrl()+'/api/chat/previous-chat',group);
  }

  public getTop2Dates(date:IDate):Observable<Date[]>{
    return this.http.post<Date[]>(this.configService.getApiUrl()+'/api/chat/dates',date);
  }

  public getNameFromStorage(){
    return sessionStorage.getItem('userName')||'';
  }

}

