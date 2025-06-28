import { Format } from './Format';
import { Stream } from './Stream';

export interface FfprobeVideoInfo {
  format: Format;
  streams: Stream[];
}
