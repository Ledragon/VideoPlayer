import { createEntityAdapter, createSelector, EntityState } from '@reduxjs/toolkit';

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
    autoCompleteLibrary: builder.mutation<string, void>({
      query: () => ({
        url: 'videos/autoComplete',
        method: 'PUT'
      }),
      invalidatesTags: ['Videos', 'ContactSheets', 'Thumbnails']
    }),
    createContactSheet: builder.mutation<void, { videoId: number; nRows: number; nCols: number }>({
      query: (params) => ({
        url: 'videos/contactSheet',
        method: 'PUT',
        body: params
      }),
      invalidatesTags: (a, b, c, d) => ['Videos', { type: 'ContactSheets', id: c.videoId }]
    }),
    metadata: builder.query<any, number>({
      query: (id) => ({
        url: 'videos/metadata',
        method: 'GET',
        params: { id }
      })
    })
  })
});

const selector = videoApi.endpoints.getVideos.select();
const videoSelectors = createSelector(selector, (videos) => videos.data);
export const {
  selectAll: getVideoList,
  selectById: getVideo,
  selectEntities: getVideoDictionary
} = entityAdapter.getSelectors<RootState>((state) => videoSelectors(state) ?? initialState);

export const { useGetVideosQuery, useUpdateVideoMutation, useCreateContactSheetMutation, useAutoCompleteLibraryMutation, useMetadataQuery } = videoApi;
