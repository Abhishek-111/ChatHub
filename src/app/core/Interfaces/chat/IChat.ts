export interface IChat{
    id?:number,
    from?:string,
    to?:string,
    content?:string,
    timeStamp:Date,
    fileId?:number,
    fileName?:string,
    fileContent?:string, // added
    fileType?:string,
    
}