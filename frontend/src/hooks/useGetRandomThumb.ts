import { useEffect, useMemo, useState } from 'react';

import { useGetThumbnailsQuery } from '../redux';
import getBase64Url from '../routes/Videos/getBase64Url';

const useGetRandomThumb = () => {
  const [background, setBackground] = useState('/TheaterCurtains.jpg');

  const { data: thumbs } = useGetThumbnailsQuery();

  const filtered = useMemo(() => {
    return thumbs?.filter((video) => video.image) || [];
  }, [thumbs]);

  useEffect(() => {
    if (filtered?.length) {
      const id = setInterval(() => {
        const video = filtered[Math.floor(Math.random() * filtered.length)];

        setBackground(getBase64Url(video.image));
      }, 3000);
      return () => clearInterval(id);
    }
  }, [filtered]);
  return background;
};

export default useGetRandomThumb;
