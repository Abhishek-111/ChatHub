import { IUserOutput } from "./IUserOutput";


export class UserOutput implements IUserOutput {

    constructor(
        public id?: number,
        public name?: string
    ) { }
}