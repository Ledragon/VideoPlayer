export enum MediaType {
  Video,
  Image
}

export interface Directory {
  id: number;
  directoryPath: string;
  directoryName: string;
  isIncludeSubdirectories: boolean;
  mediaType: MediaType;
}
