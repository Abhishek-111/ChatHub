import { IFile } from "./IFile";

/**
 * 
 */
export class File implements IFile{
    constructor(
        public fileType?:string,
        public fileContent?:string,
    ) {}
}