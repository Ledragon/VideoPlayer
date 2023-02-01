import { Video } from '../models/Video';
import api from './api';

const videoApi = api.injectEndpoints({
  endpoints: (builder) => ({
    getVideos: builder.query<Video[], void>({
      query: () => 'videos',
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
      invalidatesTags: ['Videos']
    })
  })
});

export const { useGetVideosQuery, useUpdateVideoMutation } = videoApi;
