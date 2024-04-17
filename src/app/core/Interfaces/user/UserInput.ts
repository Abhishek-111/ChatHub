import { IUserInput } from "./IUserInput";


export class UserInput implements IUserInput {

    constructor(
        public id?: number,
        public name?: string
    ) {}
}