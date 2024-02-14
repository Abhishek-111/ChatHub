import { IChat } from "./IChat";

export class Chat implements IChat{
    constructor(
        public id?:number,
        public from?:string,
        public to?:string,
        public content?:string
    ) {}
}