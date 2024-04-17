import { IConfig } from "./IConfig";

export class Config implements IConfig {
    constructor(public apiUrl: string) { }
}
  