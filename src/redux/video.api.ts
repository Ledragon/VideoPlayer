import {
  createEntityAdapter,
  createSelector,
  EntityState
} from '@reduxjs/toolkit';
import { Video } from '../models/Video';
import api from './api';
import { RootState } from './configureStore';

const entityAdapter = createEntityAdapter<Video>({
  sortComparer: (a, b) => (a.title < b.title ? -1 : 1)
});

const initialState = entityAdapter.getInitialState();

const videoApi = api.injectEndpoints({
  endpoints: (builder) => ({
    getVideos: builder.query<EntityState<Video>, void>({
      query: () => 'videos',
      transformResponse: (response: Video[]) => {
        return entityAdapter.setAll(initialState, response);
      },
      providesTags: (result) => {
        return ['Videos'];
      }
    }),
    updateVideo: builder.mutation<Video, Video>({
      query: (video) => ({
        url: 'videos',
        method: 'PUT',
        body: video
      }),
      invalidatesTags: ['Videos', 'TagVideos']
    }),
    createContactSheet: builder.mutation<
      void,
      { videoId: number; nRows: number; nCols: number }
    >({
      query: (params) => ({
        url: 'videos/contactSheet',
        method: 'PUT',
        body: params
      }),
      invalidatesTags: ['Videos']
    })
  })
});

const selector = videoApi.endpoints.getVideos.select();
const videoSelectors = createSelector(selector, (videos) => videos.data);
export const {
  selectAll: getVideoList,
  selectById: getVideo,
  selectEntities: getVideoDictionary
} = entityAdapter.getSelectors<RootState>(
  (state) => videoSelectors(state) ?? initialState
);

export const { useGetVideosQuery, useUpdateVideoMutation, useCreateContactSheetMutation } = videoApi;
