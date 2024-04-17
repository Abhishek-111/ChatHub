export interface IMessage{
    id?:number,
    fromUserName?:string,
    toUserName?:string,
    content?:string,
    timeStamp:Date
}