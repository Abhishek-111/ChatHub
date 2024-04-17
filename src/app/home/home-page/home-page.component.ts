import { Component, OnInit } from '@angular/core';
import { ChatService } from '../services/chat/chat.service';
import { IUserOutput } from 'src/app/core/Interfaces/user/IUserOutput';
import { UserService } from '../services/user/user.service';
import { IGroup } from 'src/app/core/Interfaces/group/IGroup';
import { IDate } from 'src/app/core/Interfaces/date/IDate';
import { IMessage } from 'src/app/core/Interfaces/message/IMessage';
import { IChat } from 'src/app/core/Interfaces/chat/IChat';

@Component({
  selector: 'app-home-page',
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.css']
})
export class HomePageComponent implements OnInit {

  public userId?:number;
  public userName?:string;
  public userList:IUserOutput[]=[];
  public previousChat:IChat[]=[];
  public dates:Date[]=[];

  /**
   * Creates an instance of home page component.
   * @param chatService
   * @param userService
   */
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

  /**
   * Loads the chat section component
   * @param id 
   */
  public loadChat(id?:number){
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
