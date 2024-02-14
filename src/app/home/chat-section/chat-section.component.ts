import { AfterViewChecked, AfterViewInit, Component, ElementRef, HostListener, Input, OnChanges, OnDestroy, OnInit, SimpleChanges, ViewChild } from '@angular/core';
import { ChatService } from '../services/chat/chat.service';
import { IChat } from 'src/app/core/Interfaces/chat/IChat';
import { IGroup } from 'src/app/core/Interfaces/group/IGroup';
import { IDate } from 'src/app/core/Interfaces/date/IDate';
import { IMessage } from 'src/app/core/Interfaces/message/IMessage';

@Component({
  selector: 'app-chat-section',
  templateUrl: './chat-section.component.html',
  styleUrls: ['./chat-section.component.css']
})
export class ChatSectionComponent implements OnChanges {
  @ViewChild('scrollDiv') private scrollContainer:any;
  public message:string='';
  @Input() userId?:number;
  @Input() userName?:string;
  @Input() previousChat:IMessage[]=[];
  @Input() dates:Date[]=[];
  public called:boolean=false;
  
  constructor(public chatService:ChatService){ 
    
  }
  ngAfterViewChecked(): void {
    if(!this.called && this.previousChat.length>0){
      // console.log('called');
      // console.log(this.previousChat);
      this.scrollContainer.nativeElement.scrollTop=this.scrollContainer.nativeElement.scrollHeight;
      this.called=true;
    }
   
  }

  ngOnChanges(changes: SimpleChanges): void {    
    
    this.called=false;
   // if(this.previousChat.length>0 && this.scrollContainer.nativeElement.scrollTop==0){console.log('changed');}
    
  }

  public onScroll(){
    if(this.scrollContainer.nativeElement.scrollTop==0){
      console.log('hello');
      let senderName=sessionStorage.getItem('userName')||'';
      let date:IDate={
        from:senderName,
        to:this.userName,
        passed:true,
        timeStamp:this.dates[0]
      }
      
      this.chatService.getTop2Dates(date).subscribe({
        next:(res=>{
          console.log(res);
          
          let group:IGroup={
            messageFrom:this.chatService.getNameFromStorage(),
            messageTo:this.userName,
            from:res[0],
            to:res[1]
          }
          this.scrollContainer.nativeElement.scrollTop=10;
          this.dates.unshift(...res);
          this.chatService.getPreviousChatHistory(group).subscribe({
            next:(res=>{
              this.previousChat.unshift(...res);
            })
          })
        })
      })
      
      // let senderName=sessionStorage.getItem('userName')||'';
      // let msg:IMessage={
      //   fromUserName:senderName,
      //   content:'hello',
      //   toUserName:this.userName,
      //   timeStamp:this.dates[0]
      // }
      // this.scrollContainer.nativeElement.scrollTop=65;
      // this.previousChat=[msg,...this.previousChat];
      // this.previousChat=[msg,...this.previousChat];
      
    }
  }

  public sendMessage(){
    
    let senderName=sessionStorage.getItem('userName')||'';
    let msg:IChat={
      from:senderName,
      content:this.message,
      to:this.userName,
      timeStamp:new Date()
    }
    
    this.chatService.sendPrivateMessage(msg);
  }

  public checkDate(date1:Date,date2:Date){
    date1=new Date(date1);
    date2=new Date(date2);
    if(date1.toLocaleDateString()==date2.toLocaleDateString()){
      return true;
    }
    return false;
  }
}
