import { IGroup } from "./IGroup";

export class Group implements IGroup{
    constructor(
        public messageFrom?:string,
        public messageTo?:string,
        public from?:Date,
        public to?:Date
    ) {}
}