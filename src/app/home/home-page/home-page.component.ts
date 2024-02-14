import { Component, OnDestroy, OnInit } from '@angular/core';
import { AuthService } from 'src/app/core/services/auth/auth.service';
import { ChatService } from '../services/chat/chat.service';
import { IUserOutput } from 'src/app/core/Interfaces/user/IUserOutput';
import { UserService } from '../services/user/user.service';
import { IChat } from 'src/app/core/Interfaces/chat/IChat';
import { IGroup } from 'src/app/core/Interfaces/group/IGroup';
import { IDate } from 'src/app/core/Interfaces/date/IDate';
import { IMessage } from 'src/app/core/Interfaces/message/IMessage';

@Component({
  selector: 'app-home-page',
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.css']
})
export class HomePageComponent implements OnInit {

  public userId?:number;
  public userName?:string;
  public userList:IUserOutput[]=[];
  public previousChat:IMessage[]=[];
  public dates:Date[]=[];

  constructor(private chatService:ChatService,private userService:UserService){}
  ngOnInit(): void {
   this.chatService.createConnection();
   this.userService.getAllUsers().subscribe({
    next:(res=>{
      this.userList=res;
    }),error:(err=>{
      console.log(err);
    })
   })
  }
  public send(id?:number){
    console.log(id);
    console.log(this.userId);
    // if(id && this.userId && id!=this.userId){
    //   let name=this.userList.find(user=>user.id===this.userId)?.name;
    //   let group:IGroup={
    //     messageFrom:this.chatService.getNameFromStorage(),
    //     messageTo:name
    //   }
      
    //   this.chatService.ClosePrivateChatMessage(group);
    //   console.log(group);
    // }
    this.userName=this.userList.find(user=>user.id===id)?.name;
    this.userId=id;
    
    
    var date:IDate={
      from:this.chatService.getNameFromStorage(),
      to:this.userName,
      passed:false
    }
    this.chatService.getTop2Dates(date).subscribe({
      next:(res=>{
        this.dates=res;
        console.log(res);
        let group:IGroup={
          messageFrom:this.chatService.getNameFromStorage(),
          messageTo:this.userName,
          from:this.dates[0],
          to:this.dates[1]
        }
        this.chatService.getPreviousChatHistory(group).subscribe({
          next:(res=>{
            console.log(res);
            this.previousChat=res;
          }),error:(err=>{
            this.previousChat=[];
          })
        })
      })
    })
    
  }

}
