import { AfterViewChecked, AfterViewInit, Component, ElementRef, HostListener, Input, OnChanges, OnDestroy, OnInit, SimpleChanges, ViewChild } from '@angular/core';
import { ChatService } from '../services/chat/chat.service';
import { IChat } from 'src/app/core/Interfaces/chat/IChat';
import { IGroup } from 'src/app/core/Interfaces/group/IGroup';
import { IDate } from 'src/app/core/Interfaces/date/IDate';
import { IMessage } from 'src/app/core/Interfaces/message/IMessage';
import { IFile } from 'src/app/core/Interfaces/file/IFile';

@Component({
  selector: 'app-chat-section',
  templateUrl: './chat-section.component.html',
  styleUrls: ['./chat-section.component.css']
})
export class ChatSectionComponent implements OnChanges,AfterViewChecked {
  @ViewChild('scrollDiv') private scrollContainer:any;
  @Input() userId?:number;
  @Input() userName?:string;
  @Input() previousChat:IChat[]=[];
  @Input() dates:Date[]=[];
  public message:string='';
  public called:boolean=false;
  public currentDate!:Date;
  public currentDateSet:boolean=false;
  public selectedFiles: File[] = [];
  public fileResponse: IFile = {
    fileContent:'',
    fileType:''
  }
  /**
   * Creates an instance of chat-section component.
   * @param chatService 
   */
  constructor(public chatService:ChatService){}

  /**
   * after view checked
   */
  ngAfterViewChecked(): void {
    if(!this.called && this.previousChat.length>0){
      this.scrollContainer.nativeElement.scrollTop=this.scrollContainer.nativeElement.scrollHeight;
      this.called=true;
    }
   
  }


  /**
   * on changes
   * @param changes 
   */
  ngOnChanges(changes: SimpleChanges): void {    
    this.called=false;  
  }

  /**
   * Loads the data on scroll
   */
  public onScroll(){
    if(this.scrollContainer.nativeElement.scrollTop==0){
      let senderName=sessionStorage.getItem('userName')||'';

      //send an IDate object to fetch top 2 dates
      let date:IDate={
        from:senderName,
        to:this.userName,
        passed:true,
        timeStamp:this.dates[0]
      }
      
      this.chatService.getTop2Dates(date).subscribe({
        next:(res=>{
          console.log(res);
          if(res.length>0){

            //send a IGroup object to fetch the chat between two users
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
          }
          
        })
      })
      
    }
  }

  /**
   * 
   * @param event 
   */
  onFileSelected(event: Event): void {
    const inputElement = event.target as HTMLInputElement;
    this.selectedFiles = Array.from(inputElement.files || []) as File[];
  }

  /**
   * Sends Message to a particular user
   */

  public sendMessage() {
    // check for any file attached
    if (this.selectedFiles.length > 0) {
      this.selectedFiles.forEach((selectedFile) => {
        const reader = new FileReader();
      reader.onload = (e) => {
        const fileContent = e.target?.result as string;
        const fileName = selectedFile.name;
        const fileExtension = this.getFileExtension(fileName || '');
  
        let msg: IChat = {
          from: sessionStorage.getItem('userName') || '',
          content: this.message,
          to: this.userName,
          fileContent: fileContent,
          fileType: fileExtension,
          fileName: fileName,
          timeStamp: new Date(),
        };
        // send data
        this.chatService.sendPrivateMessage(msg);
        setTimeout(() => {
          this.scrollContainer.nativeElement.scrollTop=this.scrollContainer.nativeElement.scrollHeight;
        }, 500);
        this.message = '';
      };
      reader.readAsDataURL(selectedFile);
      });
    
      // clear the files
      this.selectedFiles = [];
       
    }
    else {
      // check if message box has any content to send..
      if(this.message){
        let senderName = sessionStorage.getItem('userName') || '';
      let msg: IChat = {
        from: senderName,
        content: this.message,
        to: this.userName,
        timeStamp: new Date(),
      };

      this.chatService.sendPrivateMessage(msg);
      setTimeout(() => {
        this.scrollContainer.nativeElement.scrollTop=this.scrollContainer.nativeElement.scrollHeight;
      }, 500);
      this.message = '';
      }
    }
  }

/**
 * 
 * @param fileName 
 * @returns 
 */
getFileExtension(fileName: string): string {
  return fileName.slice(
    (Math.max(0, fileName.lastIndexOf('.')) || Infinity) + 1
  );
}



  // public sendMessage2(){
    
  //   let senderName=sessionStorage.getItem('userName')||'';
  //   let msg:IChat={
  //     from:senderName,
  //     content:this.message,
  //     to:this.userName,
  //     timeStamp:new Date()
  //   }
  //   this.chatService.sendPrivateMessage(msg);
  //   setTimeout(() => {
  //     this.scrollContainer.nativeElement.scrollTop=this.scrollContainer.nativeElement.scrollHeight;
  //   }, 100);
  //   this.message='';
  // }




  /**
   * Check the dates 
   * @param date1 
   * @param date2 
   * @returns true if dates are equal
   */
  public checkDate(currDate:Date,messageDate:Date){
    currDate=new Date(currDate);
    messageDate=new Date(messageDate);
    if(currDate.toLocaleDateString()==messageDate.toLocaleDateString()){
      return true;
    }
    return false;
  }

  /**
   * Check if date already exists 
   * @returns true if not exist
   */
  public dateExists(){

    if(!this.currentDateSet){
      let dt=new Date(this.dates[this.dates.length-1]);
      this.currentDate=new Date();
      if(this.chatService.privateMessages.length>0 && dt.toLocaleDateString()==this.currentDate.toLocaleDateString()){
        this.currentDateSet=false;
        return false;
      }
      else if(this.chatService.privateMessages.length==0){
        return false;
      }
      else{
        this.currentDateSet=true;
        return true;
      }
    }
    else{
      return true;
    }
  }


  //fild download

  /**
   * 
   * @param fileId 
   * @param fileName 
   */
  public downloadFile(fileId:number, fileName:string){
    this.chatService.getFileById(fileId.toString())
    .subscribe(response => {
      this.fileResponse = response;
      if(response){
        this.downloadNow(fileName);
      }
    },
    error => {
    })
   
  } 
 
  /**
   * 
   * @param fileName 
   */
  public downloadNow(fileName:string):void{
    const link = document.createElement('a');
    link.href = this.fileResponse.fileContent || '';
     link.download= `${fileName}`;
    link.click();
  }
}
