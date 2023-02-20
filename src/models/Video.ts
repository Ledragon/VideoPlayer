import { Tag } from './Tag';
import { Thumbnail } from './Thumbnail';

export interface Video {
  category: string;
  dateAdded: string;
  directory: string;
  fileName: string;
  id: number;
  lastPlayed: string;
  length: string;
  lengthString: string;
  numberOfViews: number;
  preview: string;
  rating: number;
  serializedImage: string;
  tags?: Tag[];
  thumbnails?: Thumbnail[];
  title: string;
}
