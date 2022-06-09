import { I } from "filepond";

declare module "filepond" {
  export interface FilePondOptions {
    /** Enable or disable file poster */
    allowFilePoster?: boolean;

    // Other FilePond plugin options here...
  }
}
