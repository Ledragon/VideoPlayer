import { Disposition } from './Disposition';

export interface Stream {
  avg_frame_rate: string;
  bit_rate: string;
  bits_per_sample: number;
  channel_layout: string;
  channels: number;
  closed_captions: number;
  codec_long_name: string;
  codec_name: string;
  codec_tag: string;
  codec_tag_string: string;
  codec_type: string;
  coded_height: number;
  coded_width: number;
  disposition: Disposition;
  duration: string;
  duration_ts: number;
  extradata_size: number;
  film_grain: number;
  has_b_frames: number;
  height: number;
  index: number;
  level: number;
  nb_frames: string;
  pix_fmt: string;
  r_frame_rate: string;
  refs: number;
  sample_fmt: string;
  sample_rate: string;
  start_pts: number;
  start_time: string;
  time_base: string;
  width: number;
}
