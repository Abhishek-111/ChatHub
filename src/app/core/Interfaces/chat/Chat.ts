import { IChat } from "./IChat";

export class Chat implements IChat{
    constructor(
        public timeStamp:Date,
        public id?:number,
        public from?:string,
        public to?:string,
        public content?:string,
        public fileId?:number,
        public fileContent?:string,
        public fileType?:string,
        public fileName?:string,
        
    ) {}
}