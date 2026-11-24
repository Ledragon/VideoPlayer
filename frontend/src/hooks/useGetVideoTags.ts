import { useMemo } from 'react';
import { useSelector } from 'react-redux';

import { getTagList, useGetTagVideosQuery } from '../redux';
import { RootState } from '../redux/configureStore';

const useGetVideoTags = (id: number | null | undefined) => {
  const tags = useSelector((state: RootState) => getTagList(state));
  const { data: tagVideos, isFetching: isLoadingTagVideos } = useGetTagVideosQuery();
  const tagsForVideo = useMemo(() => {
    return tagVideos
      ?.filter((tv) => tv.videoId === id)
      .map((tv) => tags.find((t) => t.id === tv.tagId))
      .filter((d) => !!d);
  }, [id, tagVideos, tags]);

  return tagsForVideo;
};

export default useGetVideoTags;
