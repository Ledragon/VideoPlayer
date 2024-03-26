import { createSlice } from '@reduxjs/toolkit';
import type { PayloadAction } from '@reduxjs/toolkit';

interface PlaylistState {
  id?: number;
  videos: number[];
}

const initialState: PlaylistState = {
  videos: []
};

const playlistSlice = createSlice({
  name: 'playlist',
  initialState,
  reducers: {
    addVideo: (state, action: PayloadAction<number>) => {
      if (state.videos.indexOf(action.payload) < 0) {
        state.videos.push(action.payload);
      }
    },
    addVideos: (state, action: PayloadAction<number[]>) => {
      action.payload.filter(d=>state.videos.indexOf(d) < 0)
        .forEach(d=>state.videos.push(d));
      return state;
      // if (state.videos.indexOf(action.payload) < 0) {
      //   state.videos.push(action.payload);
      // }
    },
    removeVideo: (state, action: PayloadAction<number>) => {
      const index = state.videos.indexOf(action.payload);
      if (index >= 0) {
        state.videos.splice(index, 1);
      }
    }
  }
});

export const { addVideo, addVideos, removeVideo } = playlistSlice.actions;
export default playlistSlice.reducer;
