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
    removeVideo: (state, action: PayloadAction<number>) => {
      const index = state.videos.indexOf(action.payload);
      if (index >= 0) {
        state.videos.splice(index, 1);
      }
    }
  }
});

export const { addVideo, removeVideo } = playlistSlice.actions;
export default playlistSlice.reducer;
